import type { StateCreator } from 'zustand';

export const THEME_KEY = 'theme-preference';
export const SIDEBAR_STATE_KEY = 'sidebar-state';

export interface UiStoreSliceType {
    sidebarState: 'opend' | 'closed';
    theme: 'light' | 'dark';

    toggleTheme: () => void;
    setTheme: (theme: 'light' | 'dark') => void;
    setSidebarState: (state: 'opend' | 'closed') => void;
    toggleSidebarState: () => void;
}

export const createUiSlice: StateCreator<UiStoreSliceType> = (set, get) => ({
    theme: 'dark',
    sidebarState: 'closed',

    toggleTheme: () => {
        const currentTheme = get().theme;
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        if (newTheme === 'dark') document.documentElement.classList.add('dark');
        else document.documentElement.classList.remove('dark');

        set({ theme: newTheme });

        localStorage.setItem(THEME_KEY, newTheme);
    },

    setTheme: (newTheme) => {
        if (newTheme === 'dark') document.documentElement.classList.add('dark');
        else document.documentElement.classList.remove('dark');

        set({ theme: newTheme });

        localStorage.setItem(THEME_KEY, newTheme);
    },

    setSidebarState: (state) => {
        set({ sidebarState: state });
        localStorage.setItem(SIDEBAR_STATE_KEY, state);
    },

    toggleSidebarState: () => {
        const state = get().sidebarState;
        const newState = state === 'opend' ? 'closed' : 'opend';
        set({ sidebarState: newState });
        localStorage.setItem(SIDEBAR_STATE_KEY, newState);
    },
});
