using System;
using System.Collections.Generic;
using System.IO;

using CheapLoc;

using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Command;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace HPDiff
{
	public class Plugin : IDalamudPlugin
	{
		//	Initialization
		public Plugin(
			DalamudPluginInterface pluginInterface,
			Framework framework,
			ClientState clientState,
			CommandManager commandManager,
			ObjectTable objectTable )
		{
			//	API Access
			mPluginInterface	= pluginInterface;
			mFramework			= framework;
			mClientState		= clientState;
			mCommandManager		= commandManager;
			mObjectTable		= objectTable;

			//	Configuration
			mConfiguration = mPluginInterface.GetPluginConfig() as Configuration;
			if( mConfiguration == null )
			{
				mConfiguration = new Configuration();
				mConfiguration.DiffGaugeConfigs.Add( new()
				{
					mName = "DSR - Phase 6",
					mTerritoryType = 968,
					mEnemy1Name = "Nidhogg",
					mEnemy2Name = "Hraesvelgr",
					mLeftColor = new( 163f / 255f, 73f / 255f, 164f / 255f, 1f ),
					mRightColor = new( 153f / 255f, 217f / 255f, 234f / 255f, 1f ),
					mGaugeHalfRange_Pct = 15f,
					mDiffThreshold_Pct = 3f,
				} );
			}
			mConfiguration.Initialize( mPluginInterface );

			//	Localization and Command Initialization
			OnLanguageChanged( mPluginInterface.UiLanguage );

			//	UI Initialization
			mUI = new PluginUI( this, mConfiguration );
			mPluginInterface.UiBuilder.Draw += DrawUI;
			mPluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
			mUI.Initialize();

			//	We need to disable automatic hiding, because we actually turn off our game UI nodes in the draw functions as-appropriate, so we can't skip the draw functions.
			mPluginInterface.UiBuilder.DisableAutomaticUiHide = true;

			//	Event Subscription
			mPluginInterface.LanguageChanged += OnLanguageChanged;
			mFramework.Update += OnGameFrameworkUpdate;
		}

		//	Cleanup
		public void Dispose()
		{
			mFramework.Update -= OnGameFrameworkUpdate;
			mPluginInterface.UiBuilder.Draw -= DrawUI;
			mPluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
			mPluginInterface.LanguageChanged -= OnLanguageChanged;
			mCommandManager.RemoveHandler( mTextCommandName );
			mUI.Dispose();
		}

		protected void OnLanguageChanged( string langCode )
		{
			//***** TODO *****
			var allowedLang = new List<string>{ /*"de", "ja", "fr", "it", "es"*/ };

			PluginLog.Information( "Trying to set up Loc for culture {0}", langCode );

			if( allowedLang.Contains( langCode ) )
			{
				Loc.Setup( File.ReadAllText( Path.Join( Path.Join( mPluginInterface.AssemblyLocation.DirectoryName, "Resources\\Localization\\" ), $"loc_{langCode}.json" ) ) );
			}
			else
			{
				Loc.SetupWithFallbacks();
			}

			//	Set up the command handler with the current language.
			if( mCommandManager.Commands.ContainsKey( mTextCommandName ) )
			{
				mCommandManager.RemoveHandler( mTextCommandName );
			}
			mCommandManager.AddHandler( mTextCommandName, new CommandInfo( ProcessTextCommand )
			{
				HelpMessage = "Opens the configuration window."
			} );
		}

		//	Text Commands
		protected void ProcessTextCommand( string command, string args )
		{
			if( command == mTextCommandName ) mUI.mSettingsWindowVisible = true;
		}

		public void OnGameFrameworkUpdate( Framework framework )
		{
			GaugeDrawData.mShouldDraw = false;
			for( int i = 0; i < mConfiguration.DiffGaugeConfigs.Count; ++i )
			{
				if( mClientState.TerritoryType != 0 && mConfiguration.DiffGaugeConfigs[i]?.mTerritoryType == mClientState.TerritoryType )
				{
					var enemy1 = GetEnemyForName( mConfiguration.DiffGaugeConfigs[i].mEnemy1Name ) as BattleChara;
					var enemy2 = GetEnemyForName( mConfiguration.DiffGaugeConfigs[i].mEnemy2Name ) as BattleChara;
					if( enemy1 != null && enemy2 != null )
					{
						GaugeDrawData.mShouldDraw = true;
						GaugeDrawData.mEnemy1Name = mConfiguration.DiffGaugeConfigs[i].mEnemy1Name;
						GaugeDrawData.mEnemy2Name = mConfiguration.DiffGaugeConfigs[i].mEnemy2Name;
						GaugeDrawData.mLeftColor = mConfiguration.DiffGaugeConfigs[i].mLeftColor;
						GaugeDrawData.mRightColor = mConfiguration.DiffGaugeConfigs[i].mRightColor;
						GaugeDrawData.mGaugeHalfRange_Pct = mConfiguration.DiffGaugeConfigs[i].mGaugeHalfRange_Pct;
						GaugeDrawData.mDiffThreshold_Pct = mConfiguration.DiffGaugeConfigs[i].mDiffThreshold_Pct;
						GaugeDrawData.mEnemy1HP_Pct = enemy1 != null ? (float)enemy1.CurrentHp / (float)enemy1.MaxHp * 100f : 0f;
						GaugeDrawData.mEnemy2HP_Pct = enemy2 != null ? (float)enemy2.CurrentHp / (float)enemy2.MaxHp * 100f : 0f;
						break;
					}
				}
			}
		}

		private unsafe GameObject GetEnemyForName( string name )
		{
			if( name is null or "" ) return null;

			if( FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance() != null &&
				FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule() != null &&
				FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() != null )
			{
				var atkArrayDataHolder = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule()->AtkModule.AtkArrayDataHolder;
				if( atkArrayDataHolder.NumberArrayCount >= 18 )
				{
					var pEnmityListArray = atkArrayDataHolder.NumberArrays[19];
					int enemyCount = pEnmityListArray->AtkArrayData.Size > 1 ? pEnmityListArray->IntArray[1] : 0;

					for( int i = 0; i < enemyCount; ++i )
					{
						int index = 8 + i * 6;
						if( index >= pEnmityListArray->AtkArrayData.Size ) return null;
						UInt32 OID = (UInt32)pEnmityListArray->IntArray[index];
						var gameObject = mObjectTable.SearchById( OID );
						if( gameObject?.Name.TextValue == name ) return gameObject;
					}
				}
			}

			return null;
		}

		protected void DrawUI()
		{
			mUI.Draw();
		}

		protected void DrawConfigUI()
		{
			mUI.mSettingsWindowVisible = true;
		}

		public string Name => "HP Difference Gauge";
		protected const string mTextCommandName = "/phpdiff";

		protected DalamudPluginInterface mPluginInterface;
		protected Framework mFramework;
		protected ClientState mClientState;
		protected CommandManager mCommandManager;
		protected ObjectTable mObjectTable;
		protected Configuration mConfiguration;
		protected PluginUI mUI;

		internal readonly DiffGaugeDrawData GaugeDrawData = new();
	}
}
