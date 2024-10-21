namespace Xeon.UniversalUI
{
    public class UniversalHorizontalMenu : UniversalMenuBase
    {
        protected void Move(int move)
        {
            if (!EnableInput || LockInput) return;

            selectableItems[SelectedIndex].UnSelect();
            SelectedIndex += move;
            FixIndex();
            selectableItems[SelectedIndex].Select();
            ReselectCurrentItem();
        }

        public override void Right() => Move(1);
        public override void Left() => Move(-1);
        public override void Up()
        {
            if (!EnableInput || LockInput) return;
            selectableItems[SelectedIndex].Up();
        }
        public override void Down()
        {
            if (!EnableInput || LockInput) return;
            selectableItems[SelectedIndex].Down();
        }
    }
}
