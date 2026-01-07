import { createBrowserRouter } from "react-router";
import Home from "./Home";
import { LoginPageRouters } from "./components/login/LoginPageRouters";
import { TemplatePageRouters } from "./components/template/TemplatePageRouter";

// 聚合所有路由
export const PageRouterProvider = createBrowserRouter([
  {
    path: "/",
    Component: Home,
  },
  ...LoginPageRouters,
  ...TemplatePageRouters,
]);
