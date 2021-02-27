namespace XO
{
    public static class PlayerExtensions
    {
        public static Player Reverse(this Player player)
        {
            return player == Player.X ? Player.O : Player.X;
        }
    }
}
