import { create } from 'zustand';
import { createUserSlice, type UserStoreSliceType } from './user-store';
import {
    createThemeSlice,
    THEME_KEY,
    type ThemeStoreSliceType,
} from './theme-store';

type AppStoreTupe = UserStoreSliceType & ThemeStoreSliceType;

export const useAppStore = create<AppStoreTupe>((...args) => ({
    ...createUserSlice(...args),
    ...createThemeSlice(...args),
}));

// Initialize theme from localStorage on load
if (typeof window !== 'undefined') {
    const saved = localStorage.getItem(THEME_KEY);
    useAppStore.getState().setDark(saved === 'dark');
}
