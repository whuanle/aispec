import { RefreshAccessToken } from "./components/ServiceClient";
import { IsTokenExpired } from "./helper/TokenHelper";
import useAppStore, { UserInfoModel } from "./stateshare/store";

// 登录后设置用户信息到缓存
export const SetUserInfo = (userInfo: UserInfoModel) => {
  useAppStore.getState().setUserInfo(userInfo);
};

export const GetUserInfo = (): UserInfoModel | null => {
  return useAppStore.getState().getUserInfo();
};

// 检查 accesstoken 是否有效，过期则自动刷新
export const CheckToken = async () => {
  const userInfo = GetUserInfo();
  if (!userInfo || !userInfo.accessToken) {
    console.log("No user info or access token found");
    return false;
  }

  try {
    if (IsTokenExpired(userInfo.accessToken)) {
      console.log("Access token is expired, attempting to refresh...");
      // 使用 refresh token 刷新 access token
      if (userInfo.refreshToken && !IsTokenExpired(userInfo.refreshToken)) {

        const response = await RefreshAccessToken(userInfo.refreshToken);

        // 先清理现在的用户信息
        useAppStore.getState().clearUserInfo();

        // 刷新失败
        if (!response) {
          console.log("Failed to refresh access token");
          return false;
        }

        console.log("Successfully refreshed access token");
        SetUserInfo(response);

        return true;
      } else {
        console.log("Refresh token is expired or not available");
        return false;
      }
    } else {
      console.log("Access token is still valid");
      return true;
    }
  } catch (error) {
    console.error("Error checking token:", error);
    return false;
  }
};

export const GetAccessToken = async (): Promise<string | null> => {
  const userInfo = GetUserInfo();
  if (!userInfo || !userInfo.accessToken) {
    return null;
  }

  try {
    if (IsTokenExpired(userInfo.accessToken)) {
      // 使用 refresh token 刷新 access token
      if (userInfo.refreshToken && !IsTokenExpired(userInfo.refreshToken)) {
        const response = await RefreshAccessToken(userInfo.refreshToken);

        // 刷新失败
        if (!response) {
          return null;
        }

        SetUserInfo(response);

        return response.accessToken!;
      }
    } else {
      return userInfo.accessToken;
    }
  } catch (error) {
    console.error("Error checking token:", error);
    return null;
  }

  return null;
};
