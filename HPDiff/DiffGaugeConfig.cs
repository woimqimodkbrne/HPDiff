using System.Numerics;

namespace HPDiff;

public class DiffGaugeConfig
{
	public string mName = "Gauge";
	public bool mEnabled = true;
	public int mTerritoryType;
	public string mEnemy1Name = "";
	public string mEnemy2Name = "";
	public Vector4 mLeftColor = new( 163f/255f, 73f/255f, 164f/255f, 1f );
	public Vector4 mRightColor = new( 153f/255f, 217f/255f, 234f/255f, 1f );
	public float mGaugeHalfRange_Pct = 15f;
	public float mDiffThreshold_Pct = 3f;

	public DiffGaugeConfig(){}
	public DiffGaugeConfig( DiffGaugeConfig objToCopy )
	{
		mName = objToCopy.mName;
		mEnabled = objToCopy.mEnabled;
		mTerritoryType = objToCopy.mTerritoryType;
		mEnemy1Name = objToCopy.mEnemy1Name;
		mEnemy2Name = objToCopy.mEnemy2Name;
		mLeftColor = objToCopy.mLeftColor;
		mRightColor = objToCopy.mRightColor;
		mGaugeHalfRange_Pct = objToCopy.mGaugeHalfRange_Pct;
		mDiffThreshold_Pct = objToCopy.mDiffThreshold_Pct;
	}
}
