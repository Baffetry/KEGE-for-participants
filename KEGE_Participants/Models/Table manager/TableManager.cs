using System.Windows.Controls;

namespace KEGE_Participants.Models.Table_manager
{
    public class TableManager(Grid grid)
    {
        public readonly Grid _grid = grid;

        public List<string> ExtractAnswers()
        {
            var result = new List<string>();

            for (int row = 0; row < _grid.RowDefinitions.Count; row++)
            {
                for (int col = 1; col <= 2; col++)
                {
                    var tb = GetTextBoxAt(row, col);

                    string text = tb?.Text?.Trim();

                    result.Add(string.IsNullOrEmpty(text) ? "%noAnswer%" : text);
                }
            }

            while (result.Count > 0)
            {
                int last = result.Count - 1;
                int secondLast = result.Count - 2;

                if (result[last].Equals("%noAnswer%") && result[secondLast].Equals("%noAnswer%"))
                    result.RemoveRange(secondLast, 2);
                else
                    break;
            }

            if (result.Count == 0)
            {
                result.Clear();
                result.Add("%noAnswer%");
            }    

            return result;
        }

        public void PasteDate(string[] data, int startRow, int startCol)
        {
            int dataIndex = 0;

            for (int row = startRow; row < _grid.RowDefinitions.Count; row++)
            {
                int idx = (row == startRow) ? startCol : 1;

                for (int col = idx; col <= 2; col++)
                {
                    if (dataIndex >= data.Length) return;

                    var tb = GetTextBoxAt(row, col);
                    if (tb is not null) tb.Text = data[dataIndex++];
                }
            }
        }

        public void RestoreTable(string participantAnswer)
        {
            if (string.IsNullOrWhiteSpace(participantAnswer)) return;

            string[] parts = participantAnswer.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            PasteDate(parts, 0, 1);
        }

        private TextBox GetTextBoxAt(int row, int col)
        {
            return _grid.Children
                   .OfType<Border>()
                   .FirstOrDefault(b => Grid.GetRow(b) == row && Grid.GetColumn(b) == col)?
                   .Child as TextBox;
        }
    }
}
