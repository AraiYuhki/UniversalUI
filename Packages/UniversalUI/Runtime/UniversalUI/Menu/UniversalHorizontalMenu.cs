public class UniversalHorizontalMenu : UniversalMenuBase
{
    protected void Move(int move)
    {
        items[selectedIndex].UnSelect();
        selectedIndex += move;
        FixIndex();
        items[selectedIndex].Select();
        ReselectCurrentItem();
    }

    public override void Right() => Move(1);
    public override void Left() => Move(-1);
    public override void Up()
    {
        if (!EnableInput) return;
        items[selectedIndex].Up();
    }
    public override void Down()
    {
        if (!EnableInput) return;
        items[selectedIndex].Down();
    }
}
