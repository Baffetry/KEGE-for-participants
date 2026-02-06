using KEGE_Participants.User_Controls;
using Participant_Result;
using System.Windows.Controls;

namespace KEGE_Participants
{
    public class ResultCollector
    {
        private static ResultCollector _instance;
        public static ResultCollector Instance = _instance ??= new ResultCollector();

        public string OptionId { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public List<Answer> Answers { get; set; } = new();

        public Result GetResult()
        {
            var result = new Result();

            result.OptionID = OptionId;
            result.Name = Name;
            result.SecondName = SecondName;
            result.MiddleName = MiddleName;

            foreach (var answer in Answers)
                result.Answers.Add(answer);

            return result;
        }

        public void SetAnswers(Dictionary<string, TaskViewControl> collection)
        {
            foreach (var panel in collection.Values)
            {
                string taskNumber = panel.TaskId;
                string response = panel.GetAnswer();

                Answers.Add(new Answer(taskNumber, response));
            }
        }
    }
}
