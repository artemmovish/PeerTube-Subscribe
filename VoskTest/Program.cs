using Vosk;
using NAudio.Wave;
using System;

class Program
{
    static void Main()
    {
        string modelPath = @"E:\Project\Trainee\C#\PeerTube-Subscribe\VoskTest\vosk-model-en-us-0.22-lgraph\";
        string audioFilePath = @"E:\Project\Trainee\C#\PeerTube-Subscribe\VoskTest\Sounds\male.wav";

        Vosk.Vosk.SetLogLevel(0);
        Model model = new Model(modelPath);

        using (var waveReader = new WaveFileReader(audioFilePath))
        {
            // Проверяем исходный формат
            Console.WriteLine($"Исходный формат: {waveReader.WaveFormat.SampleRate} Hz, {waveReader.WaveFormat.Channels} каналов");

            // Если частота не 16 кГц, ресемплируем
            var targetFormat = new WaveFormat(16000, 1); // 16 кГц, моно
            using (var resampler = new MediaFoundationResampler(waveReader, targetFormat))
            {
                var recognizer = new VoskRecognizer(model, 16000.0f);

                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        Console.WriteLine(recognizer.Result());
                    }
                    else
                    {
                        Console.WriteLine(recognizer.PartialResult());
                    }
                }

                Console.WriteLine(recognizer.FinalResult());
            }
        }
    }
}