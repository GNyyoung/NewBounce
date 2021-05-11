namespace MapEditor
{
    public class EditorDecoration : EditorObject
    {
        public override bool Remove()
        {
            Destroy(this.gameObject);
            return true;
        }
    }
}