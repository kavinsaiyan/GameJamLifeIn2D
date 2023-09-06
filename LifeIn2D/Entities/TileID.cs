namespace LifeIn2D.Entities
{
    public enum TileID
    {
        None = -1,
        Horizontal = 0,
        Vertical = 1,
        Plus = 2,
        L_normal = 3,//bottom left cornor
        L_rot90 = 4,//top left cornor
        L_rot180 = 5,//top right cornor
        L_rot270 = 6,//bottom right cornor
        Threeway_normal = 7,
        Threeway_rot90 = 8,
        Threeway_rot180 = 9,
        Threeway_rot270 = 10,
        Heart = 11,
        Brain = 12,
        Kidney = 13,
        Lungs = 14,
        Intestine = 15,
        Dest_Down =16,
        Dest_Up = 17,
        Dest_Right = 18,
        Dest_Left = 19,
    }
}