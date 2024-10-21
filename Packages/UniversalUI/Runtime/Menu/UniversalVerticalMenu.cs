namespace Xeon.UniversalUI
{
    public class UniversalVerticalMenu : UniversalMenuBase
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

        public override void Up() => Move(-1);
        public override void Down() => Move(1);
        public override void Right()
        {
            if (!EnableInput || LockInput) return;
            selectableItems[SelectedIndex].Right();
        }
        public override void Left()
        {
            if (!EnableInput || LockInput) return;
            selectableItems[SelectedIndex].Left();
        }
    }
}
