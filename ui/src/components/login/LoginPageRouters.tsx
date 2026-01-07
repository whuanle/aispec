import { RouteObject } from "react-router";
import Login from "./Login";

export const LoginPageRouters: RouteObject[] = [
  {
    path: "/admin",
    children: [
      {
        index: true,
        Component: Login, // 暂时指向登录页，后续可替换为后台首页
      },
      {
        path: "login",
        Component: Login,
      },
    ],
  },
];
