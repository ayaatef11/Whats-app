namespace WhatsApp.Events
{
    public record DragEvent(int CurrentPos, int OriginalPos);

    public record UserDragEvent(DragEvent Drag, string Username, string Colour)
        : DragEvent(Drag.CurrentPos, Drag.OriginalPos);


}
