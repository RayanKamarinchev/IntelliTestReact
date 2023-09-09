using IntelliTest.Core.Models.Questions;
using IntelliTest.Core.Models.Tests;

namespace IntelliTest.Core.Contracts
{
    public interface ITestResultsService
    {
        //Answer processing
        Task AddTestAnswer(List<OpenQuestionAnswerViewModel> openQuestions,
                           List<ClosedQuestionAnswerViewModel> closedQuestions,
                           int studentId,
                           int testId);
        decimal CalculateClosedQuestionScore(bool[] Answers, int[] RightAnswers, int MaxScore);
        bool[] ProccessAnswerIndexes(string[] answers, string answerIndexes);
        TestEditViewModel ToEdit(TestViewModel model);
        //Statistics and results
        int[] GetExaminersIds(int testId);
        Task<TestStatsViewModel> GetStatistics(int testId);
        public Task<List<TestStatsViewModel>> TestsTakenByClass(int classId);
        Task<TestReviewViewModel> GetStudentsTestResults(int testId, int studentId);
        Task<IEnumerable<TestResultsViewModel>> GetStudentsTestsResults(int studentId);
        Task SubmitTestScore(int testId, int studentId, TestReviewViewModel scoredTest);
    }
}