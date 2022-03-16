// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.34
// 

using Colyseus.Schema;

public partial class LeaderboardRoomState : Schema {
	[Type(0, "array", typeof(ArraySchema<LeaderboardData>))]
	public ArraySchema<LeaderboardData> datas = new ArraySchema<LeaderboardData>();

	[Type(1, "ref", typeof(LeaderboardData))]
	public LeaderboardData playerData = new LeaderboardData();
}

