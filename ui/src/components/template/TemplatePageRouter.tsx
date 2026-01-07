import { RouteObject } from 'react-router';
import TemplatePage from './TemplatePage';

export const TemplatePageRouters: RouteObject[] = [
  {
    path: 'template',
    element: <TemplatePage />,
  },
];
