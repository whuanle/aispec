import React from 'react';
import { Card, Typography, Button, Space } from 'antd';
import { HomeOutlined, SettingOutlined } from '@ant-design/icons';
import './TemplatePage.css';

const { Title, Paragraph } = Typography;

const TemplatePage: React.FC = () => {
  return (
    <div className="page-container">
      <div className="moai-page-header">
        <Title level={2} className="moai-page-title">模板页面</Title>
        <Paragraph className="moai-page-subtitle">
          这是一个基础模板页面，你可以基于此创建新页面
        </Paragraph>
      </div>

      <div className="moai-toolbar">
        <div className="moai-toolbar-left">
          <Space>
            <Button icon={<HomeOutlined />}>操作按钮</Button>
          </Space>
        </div>
        <div className="moai-toolbar-right">
          <Button type="primary" icon={<SettingOutlined />}>
            主要操作
          </Button>
        </div>
      </div>

      <Card className="moai-card">
        <Title level={4}>内容区域</Title>
        <Paragraph>
          在这里添加你的页面内容。项目使用 Ant Design 5 作为 UI 组件库，
          支持 @lobehub/ui 主题系统。
        </Paragraph>
        <Paragraph>
          API 调用请使用 <code>GetApiClient()</code> 获取已配置认证的客户端。
        </Paragraph>
      </Card>
    </div>
  );
};

export default TemplatePage;
