using System.Diagnostics;
using System.IO;
using Task_Data;

namespace KEGE_Participants.Models.File_manager
{
    public class FileManager
    {
        public void OpenFile(FileData file)
        {
            if (file == null || file.Data == null) return;

            try
            {
                // Создаем путь во временной директории пользователя
                string tempFolder = Path.Combine(Path.GetTempPath(), "EgeClient_Cache");
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                string filePath = Path.Combine(tempFolder, file.FileName);

                // Записываем данные в файл 
                File.WriteAllBytes(filePath, file.Data);

                // Открываем файл программой по умолчанию
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Это заставит Windows искать сопоставленное приложение
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
        }
    }
}
