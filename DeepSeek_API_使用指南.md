# DeepSeek API 使用指南和故障排除

## DeepSeek API 基本信息

### API 端点
- 基础 URL: `https://api.deepseek.com`
- Chat Completions: `/v1/chat/completions`

### 认证
- 使用 Bearer Token 认证
- Header: `Authorization: Bearer YOUR_API_KEY`

### 支持的模型
- `deepseek-chat`: 通用对话模型
- `deepseek-coder`: 代码专用模型

## 请求格式

### 标准请求示例
```json
{
  "model": "deepseek-chat",
  "messages": [
    {
      "role": "system",
      "content": "你是一个专业的桌面文件整理助手。"
    },
    {
      "role": "user", 
      "content": "请帮我整理桌面文件..."
    }
  ],
  "stream": false,
  "temperature": 0.1,
  "max_tokens": 4000,
  "top_p": 0.8
}
```

### 流式请求
```json
{
  "model": "deepseek-chat",
  "messages": [...],
  "stream": true
}
```

## 响应格式

### 非流式响应
```json
{
  "id": "chatcmpl-xxx",
  "object": "chat.completion",
  "created": 1699896916,
  "model": "deepseek-chat",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "响应内容..."
      },
      "finish_reason": "stop"
    }
  ],
  "usage": {
    "prompt_tokens": 100,
    "completion_tokens": 200,
    "total_tokens": 300
  }
}
```

### 流式响应
```
data: {"choices": [{"delta": {"content": "响应"}}]}
data: {"choices": [{"delta": {"content": "内容"}}]}
data: [DONE]
```

## 常见错误和解决方案

### 1. 401 Unauthorized
**原因**: API 密钥无效或未设置
**解决**: 
- 检查 API 密钥是否正确
- 确保密钥格式为 `sk-xxx`
- 在模型配置中正确设置密钥引用

### 2. 429 Too Many Requests
**原因**: 请求频率过高
**解决**:
- 增加请求间隔
- 检查是否有并发请求
- 升级 API 套餐

### 3. 400 Bad Request
**原因**: 请求格式错误
**解决**:
- 检查 JSON 格式是否正确
- 确保必需字段存在
- 检查参数值是否在有效范围内

### 4. 超时错误
**原因**: 网络连接问题或服务器响应慢
**解决**:
- 增加超时时间
- 检查网络连接
- 重试请求

## 应用程序配置检查清单

### 1. API 密钥配置
- [ ] 在模型配置中设置了正确的 API 密钥
- [ ] API 密钥格式正确 (sk-xxx)
- [ ] 密钥已保存到 Windows 凭据管理器

### 2. 模型配置
- [ ] 基础 URL: `https://api.deepseek.com`
- [ ] 模型 ID: `deepseek-chat` 或 `deepseek-coder`
- [ ] 超时时间: 建议 60-120 秒
- [ ] 密钥引用: 与保存的凭据名称一致

### 3. 网络配置
- [ ] 确保可以访问 `api.deepseek.com`
- [ ] 检查防火墙和代理设置
- [ ] 测试网络连接

## 调试步骤

### 1. 启用详细日志
应用程序已配置详细的日志记录，在 Debug 模式下会输出：
- API 请求详情
- 响应状态和内容
- 错误详细信息

### 2. 检查日志输出
查看 Visual Studio 输出窗口或控制台，寻找：
- `DeepSeek API` 相关的日志条目
- HTTP 状态码
- 错误消息详情

### 3. 测试连接
使用应用程序的"测试连接"功能验证：
- API 密钥有效性
- 网络连接状态
- 模型配置正确性

## 故障排除流程

1. **检查 API 密钥**
   - 确认密钥是否正确设置
   - 测试密钥是否有效

2. **验证网络连接**
   - 尝试直接访问 DeepSeek API
   - 检查防火墙设置

3. **查看详细日志**
   - 启用 Debug 模式
   - 查看完整的错误信息

4. **测试简单请求**
   - 使用最小化的请求进行测试
   - 逐步增加复杂度

5. **联系支持**
   - 如果问题持续，查看 DeepSeek 官方文档
   - 联系 DeepSeek 技术支持

## 性能优化建议

1. **合理设置超时时间**
   - 根据请求复杂度调整超时时间
   - 避免设置过短的超时

2. **使用流式传输**
   - 对于长响应使用流式传输
   - 提供实时反馈给用户

3. **错误重试机制**
   - 对临时性错误进行重试
   - 使用指数退避策略

4. **请求优化**
   - 精简提示内容
   - 合理设置 max_tokens 参数