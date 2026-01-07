import { create } from 'zustand'

export interface UserInfoModel {
  /**
   * 访问令牌.
   */
  accessToken?: string | null;
  /**
   * 过期时间（秒）.
   */
  expiresIn?: string | null;
  /**
   * 刷新令牌.
   */
  refreshToken?: string | null;
  /**
   * 令牌类型.
   */
  tokenType?: string | null;
  /**
   * 用户ID.
   */
  userId?: number | null;
  /**
   * 用户名.
   */
  userName?: string | null;
}

/**
 * 用户详细信息（从 API 获取）
 */
export interface UserDetailInfo {
  userName?: string | null;
  nickName?: string | null;
  avatar?: string | null;
}

// 主题类型
export type ThemeMode = 'light' | 'dark';

interface AppState {
  userInfo: UserInfoModel | null
  userDetailInfo: UserDetailInfo | null
  themeMode: ThemeMode
  
  // User info actions
  setUserInfo: (userInfo: UserInfoModel) => void
  getUserInfo: () => UserInfoModel | null
  clearUserInfo: () => void
  
  // User detail info actions
  setUserDetailInfo: (userDetailInfo: UserDetailInfo) => void
  clearUserDetailInfo: () => void
  
  // Theme actions
  setThemeMode: (mode: ThemeMode) => void
  toggleTheme: () => void
}

// 获取初始主题
const getInitialTheme = (): ThemeMode => {
  const saved = localStorage.getItem('theme.mode');
  if (saved === 'light' || saved === 'dark') return saved;
  // 跟随系统偏好
  if (window.matchMedia?.('(prefers-color-scheme: dark)').matches) {
    return 'dark';
  }
  return 'light';
};

const useAppStore = create<AppState>((set, get) => ({
  userInfo: null,
  userDetailInfo: null,
  themeMode: getInitialTheme(),
  
  // User info actions
  setUserInfo: (userInfo: UserInfoModel) => {
    // Save to localStorage
    if (userInfo.accessToken) localStorage.setItem("userinfo.accessToken", userInfo.accessToken);
    if (userInfo.expiresIn) localStorage.setItem("userinfo.expiresIn", userInfo.expiresIn);
    if (userInfo.refreshToken) localStorage.setItem("userinfo.refreshToken", userInfo.refreshToken);
    if (userInfo.tokenType) localStorage.setItem("userinfo.tokenType", userInfo.tokenType);
    if (userInfo.userId) localStorage.setItem("userinfo.userId", userInfo.userId.toString());
    if (userInfo.userName) localStorage.setItem("userinfo.userName", userInfo.userName);
    
    set({ userInfo });
  },
  
  getUserInfo: () => {
    const state = get();
    if (state.userInfo) {
      return state.userInfo;
    }
    
    // Try to load from localStorage
    const accessToken = localStorage.getItem("userinfo.accessToken");
    const expiresIn = localStorage.getItem("userinfo.expiresIn");
    const refreshToken = localStorage.getItem("userinfo.refreshToken");
    const tokenType = localStorage.getItem("userinfo.tokenType");
    const userIdStr = localStorage.getItem("userinfo.userId");
    const userName = localStorage.getItem("userinfo.userName");
    
    if (accessToken || refreshToken) {
      const userInfo: UserInfoModel = {
        accessToken,
        expiresIn,
        refreshToken,
        tokenType,
        userId: userIdStr ? Number(userIdStr) : null,
        userName
      };
      set({ userInfo });
      return userInfo;
    }
    
    return null;
  },
  
  clearUserInfo: () => {
    localStorage.removeItem("userinfo.accessToken");
    localStorage.removeItem("userinfo.expiresIn");
    localStorage.removeItem("userinfo.refreshToken");
    localStorage.removeItem("userinfo.tokenType");
    localStorage.removeItem("userinfo.userId");
    localStorage.removeItem("userinfo.userName");
    set({ userInfo: null });
  },
  
  // User detail info actions
  setUserDetailInfo: (userDetailInfo: UserDetailInfo) => {
    set({ userDetailInfo });
  },
  
  clearUserDetailInfo: () => {
    set({ userDetailInfo: null });
  },
  
  // Theme actions
  setThemeMode: (mode: ThemeMode) => {
    localStorage.setItem('theme.mode', mode);
    set({ themeMode: mode });
  },
  
  toggleTheme: () => {
    const current = get().themeMode;
    const newMode = current === 'light' ? 'dark' : 'light';
    localStorage.setItem('theme.mode', newMode);
    set({ themeMode: newMode });
  },
}))

export default useAppStore
