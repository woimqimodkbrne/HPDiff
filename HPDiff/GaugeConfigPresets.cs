namespace HPDiff;

internal static class GaugeConfigPresets
{
	internal static readonly DiffGaugeConfig DSR_Phase6 = new ()
	{
		mName = "DSR - Phase 6",
		mEnabled = true,
		mTerritoryType = 968,
		mEnemy1Name = "Nidhogg",
		mEnemy2Name = "Hraesvelgr",
		mLeftColor = new ( 163f / 255f, 73f / 255f, 164f / 255f, 1f ),
		mRightColor = new ( 153f / 255f, 217f / 255f, 234f / 255f, 1f ),
		mGaugeHalfRange_Pct = 15f,
		mDiffThreshold_Pct = 3f,
	};

	internal static readonly DiffGaugeConfig TEA_Phase1 = new ()
	{
		mName = "TEA - Phase 1",
		mEnabled = true,
		mTerritoryType = 887,
		mEnemy1Name = "Living Liquid",
		mEnemy2Name = "Liquid Hand",
		mLeftColor = new ( 100f / 255f, 210f / 255f, 239f / 255f, 1f ),
		mRightColor = new ( 98f / 255f, 122f / 255f, 145f / 255f, 1f ),
		mGaugeHalfRange_Pct = 20f,
		mDiffThreshold_Pct = 5f,
	};

	internal static readonly DiffGaugeConfig DragonsNeck_Testing = new ()
	{
		mName = "Dragon's Neck (Testing)",
		mEnabled = true,
		mTerritoryType = 142,
		mEnemy1Name = "Ultros",
		mEnemy2Name = "Typhon",
		mLeftColor = new ( 140f / 255f, 58f / 255f, 141f / 255f, 1f ),
		mRightColor = new ( 240f / 255f, 166f / 255f, 220f / 255f, 1f ),
		mGaugeHalfRange_Pct = 20f,
		mDiffThreshold_Pct = 5f,
	};
}
