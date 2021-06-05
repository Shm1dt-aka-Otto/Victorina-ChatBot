namespace QuizLogic
{
    public class QuestionState
    {
        public QuestionItem CurrentItem { get; set; }
        public int Opened { get; set; }
        public string AnswerHint => CurrentItem.Answer
            .Substring(0, Opened)
            .PadRight(CurrentItem.Answer.Length, '_');
        public string DispayQuestion => $"{CurrentItem.Question}: {CurrentItem.Answer.Length} букв\n{AnswerHint}";
        public bool IsEnd => Opened == CurrentItem.Answer.Length;
    }
}
