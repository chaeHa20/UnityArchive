public enum eTable
{
    None,
    // add your customTable tag
}

public class GameTableManager : TableManager<GameTableManager>
{
    public override void initialize(Crypto crypto)
    {
        base.initialize(crypto);
    }

    protected override void load(Crypto crypto)
    {
        base.load(crypto);


        // TODO : call load functiom
        /// <summary>
        /// 예를 들어 ChartDataTable이라고 할 때
        /// load<ChartDataTable>((int)eTable.테이블 테그, "Table/테이블 이름.txt", crypto);
        /// </summary>
    }
}
