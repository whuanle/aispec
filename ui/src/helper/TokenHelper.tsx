import { jwtDecode } from "jwt-decode";

interface JwtPayload {
  exp: number;
}

// 检查 token 是否过期
export function IsTokenExpired(token: string): boolean {
  try {
    const decoded = jwtDecode<JwtPayload>(token);

    const currentTime = Math.floor(Date.now() / 1000);

    // 倒退一分钟，避免刷新来不及
    return decoded.exp < currentTime - 60;
  } catch (error) {
    // 如果无法解码 token，认为 token 已过期或无效
    console.error("Invalid Token:", error);
    return true;
  }
}
