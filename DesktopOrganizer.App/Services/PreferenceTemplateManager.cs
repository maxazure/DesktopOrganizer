using DesktopOrganizer.Domain;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DesktopOrganizer.App.Services;

/// <summary>
/// Manages preference templates for natural language organization preferences
/// </summary>
public class PreferenceTemplateManager
{
    private readonly ILogger<PreferenceTemplateManager> _logger;
    private readonly string _templatesFilePath;
    private List<PreferenceTemplate> _templates = new();

    public PreferenceTemplateManager(ILogger<PreferenceTemplateManager> logger)
    {
        _logger = logger;
        _templatesFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DesktopOrganizer",
            "templates.json"
        );
        
        LoadTemplates();
    }

    public List<PreferenceTemplate> Templates => _templates.ToList();

    public void LoadDefaultTemplates()
    {
        _templates.Clear();
        
        // Add built-in templates
        _templates.AddRange(new[]
        {
            new PreferenceTemplate(
                "基础整理",
                "根据文件类型进行基础分类",
                "请将我的文件按照类型分类：图片放到'图片'文件夹，文档放到'文档'文件夹，视频放到'视频'文件夹，音频放到'音频'文件夹，其他文件放到'其他'文件夹。",
                true
            ),
            new PreferenceTemplate(
                "工作优先",
                "优先整理工作相关文件",
                "请优先整理工作相关的文件，将PPT、Word、Excel文件放到'工作文档'文件夹，PDF文件放到'参考资料'文件夹，图片和视频按项目分类整理。",
                true
            ),
            new PreferenceTemplate(
                "学习资料",
                "适用于学习资料的整理",
                "请将学习资料分类整理：课程文档放到'课程资料'文件夹，笔记和作业放到'学习笔记'文件夹，参考书籍和论文放到'参考文献'文件夹。",
                true
            ),
            new PreferenceTemplate(
                "媒体文件",
                "专门整理图片、音频、视频文件",
                "请将媒体文件按时间和类型整理：照片按年月分类，视频按类型分类（电影、电视剧、教程等），音乐按专辑或艺术家分类。",
                true
            ),
            new PreferenceTemplate(
                "项目管理",
                "按项目和任务分类文件",
                "请按项目将文件分类整理，每个项目创建独立文件夹，项目内再按文档类型细分（设计稿、代码、文档、素材等）。",
                true
            )
        });

        _logger.LogInformation("Loaded {Count} default templates", _templates.Count);
    }

    public void LoadTemplates()
    {
        try
        {
            if (!File.Exists(_templatesFilePath))
            {
                LoadDefaultTemplates();
                SaveTemplates();
                return;
            }

            var json = File.ReadAllText(_templatesFilePath);
            var templates = JsonSerializer.Deserialize<List<PreferenceTemplate>>(json);
            
            if (templates != null)
            {
                _templates = templates;
                _logger.LogInformation("Loaded {Count} templates from file", _templates.Count);
            }
            else
            {
                LoadDefaultTemplates();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load templates, using defaults");
            LoadDefaultTemplates();
        }
    }

    public void SaveTemplates()
    {
        try
        {
            var directory = Path.GetDirectoryName(_templatesFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            var json = JsonSerializer.Serialize(_templates, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            File.WriteAllText(_templatesFilePath, json);
            _logger.LogInformation("Saved {Count} templates to file", _templates.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save templates");
        }
    }

    public void SaveCustomTemplate(string name, string content, string description = "")
    {
        var existingTemplate = _templates.FirstOrDefault(t => t.Name == name && !t.IsBuiltIn);
        
        if (existingTemplate != null)
        {
            existingTemplate.Content = content;
            existingTemplate.Description = description;
            existingTemplate.LastModified = DateTime.Now;
        }
        else
        {
            _templates.Add(new PreferenceTemplate(name, description, content, false));
        }

        SaveTemplates();
        _logger.LogInformation("Saved custom template: {Name}", name);
    }

    public void DeleteTemplate(string name)
    {
        var template = _templates.FirstOrDefault(t => t.Name == name);
        if (template != null && !template.IsBuiltIn)
        {
            _templates.Remove(template);
            SaveTemplates();
            _logger.LogInformation("Deleted template: {Name}", name);
        }
        else
        {
            _logger.LogWarning("Cannot delete built-in template or template not found: {Name}", name);
        }
    }

    public PreferenceTemplate? GetTemplate(string name)
    {
        return _templates.FirstOrDefault(t => t.Name == name);
    }

    public List<PreferenceTemplate> GetCustomTemplates()
    {
        return _templates.Where(t => !t.IsBuiltIn).ToList();
    }

    public List<PreferenceTemplate> GetBuiltInTemplates()
    {
        return _templates.Where(t => t.IsBuiltIn).ToList();
    }
}