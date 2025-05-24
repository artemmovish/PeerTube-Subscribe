using Vosk;
using NAudio.Wave;
using System;
using Microsoft.VisualBasic;
using System.Reflection;

class Program
{
    static void Main()
    {
        Vosk.Vosk.SetLogLevel(0); // Отключает логгирование
        Model model = new Model("models/vosk-model-small-ru-0.22");
        using var waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(16000, 1);
        var recognizer = new VoskRecognizer(model, 16000.0f);

        waveIn.DataAvailable += (s, a) =>
        {
            if (recognizer.AcceptWaveform(a.Buffer, a.BytesRecorded))
                Console.WriteLine(recognizer.Result());
            else
                Console.WriteLine(recognizer.PartialResult());
        };

        waveIn.StartRecording();
        Console.WriteLine("Говорите... (нажмите любую клавишу для завершения)");
        Console.ReadKey();
        waveIn.StopRecording();
        Console.WriteLine(recognizer.FinalResult());
    }
}
