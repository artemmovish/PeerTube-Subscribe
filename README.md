# 🧱 Описание сервисов и их связей
| Сервис               | Основная роль                                                                 |
|:---------------------|:------------------------------------------------------------------------------|
| VideoUploadService   | UI/API для загрузки видео. Принимает параметры: язык, публичность, тег и т.д. |
| TranscriptionService | Расшифровка видео через AI-движок. Хранит TranscriptionJob.                  |
| SubtitleService      | Конвертирует TranscriptResult в .srt (или .vtt)                              |
| PublishingService    | Публикует видео, субтитры, описание в PeerTube через REST API                |



# 📦 1. Video (в VideoUploadService.Domain.Entities)
```
public class Video
{
    public Guid Id { get; set; }
    public string FilePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
    public List<string> Tags { get; set; }
    public DateTime UploadedAt { get; set; }
    public VideoStatus Status { get; set; } // Uploaded, Transcribed, Subtitled, Published
}
```

# 🧠 2. TranscriptionJob (в TranscriptionService.Domain.Entities)
```
public class TranscriptionJob
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string AudioPath { get; set; }
    public string TranscriptPath { get; set; }
    public string TranscriptText { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TranscriptionStatus Status { get; set; } // Pending, InProgress, Completed, Failed
}
```

# 📝 3. Subtitle (в SubtitleService.Domain.Entities)
```
public class Subtitle
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string SubtitlePath { get; set; } // путь к .srt файлу
    public DateTime CreatedAt { get; set; }
}
```

# 🚀 4. PublishRequest (в PublishingService.Domain.Entities)
```
hpublic class PublishRequesta
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string VideoPath { get; set; }
    public string SubtitlePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
    public List<string> Tags { get; set; }
    public PublishStatus Status { get; set; } // Pending, Publishing, Published, Failed
    public DateTime CreatedAt { get; set; }
}
```

# 🔄 Поток данных
##	Загрузка видео (VideoUploadService)
-	принимает файл
-	отправляет событие (через брокер, например, Kafka) о новом видео
##	Транскрипция (TranscriptionService)
-	получает видео, извлекает аудио
-   расшифровывает через Vosk
-	отдает текст и временные метки
##	Субтитры (SubtitleService)
-	получает текст + тайминги
-	формирует .srt
##	Публикация (PublishingService)
-	загружает видео + .srt в PeerTube (REST API)
-	добавляет описание, теги и прочее

