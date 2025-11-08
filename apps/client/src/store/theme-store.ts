import type { StateCreator } from 'zustand';

export interface ThemeStoreSliceType {
    dark: boolean;
    toggle: () => void;
    setDark: (value: boolean) => void;
}

export const THEME_KEY = 'theme-preference';

export const createThemeSlice: StateCreator<ThemeStoreSliceType> = (set) => ({
    dark: false,

    toggle: () =>
        set((state) => {
            const newTheme = !state.dark;
            localStorage.setItem(THEME_KEY, newTheme ? 'dark' : 'light');
            if (newTheme) document.documentElement.classList.add('dark');
            else document.documentElement.classList.remove('dark');
            return { dark: newTheme };
        }),

    setDark: (value: boolean) => {
        localStorage.setItem(THEME_KEY, value ? 'dark' : 'light');
        if (value) document.documentElement.classList.add('dark');
        else document.documentElement.classList.remove('dark');
        set({ dark: value });
    },
});
