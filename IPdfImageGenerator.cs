namespace ImageToPDF
{
    interface IPdfImageGenerator
    {
        bool IsValidCommand(TaskCommand command);
        void SaveAsPdf(TaskCommand command);
    }
}
