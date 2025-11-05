import { create } from 'zustand';
import { createUserSlice, type UserStoreSliceType } from './user-store';
import { createUiSlice, THEME_KEY, type UiStoreSliceType } from './ui-store';
import { createSkillSlice, type SkillStoreSliceType } from './skill-store';

type AppStoreTupe = UserStoreSliceType & UiStoreSliceType & SkillStoreSliceType;

export const useAppStore = create<AppStoreTupe>((...args) => ({
    ...createUserSlice(...args),
    ...createUiSlice(...args),
    ...createSkillSlice(...args),
}));

// Initialize theme from localStorage on load
if (typeof window !== 'undefined') {
    const saved = localStorage.getItem(THEME_KEY);
    useAppStore.getState().setDark(saved === 'dark');
}
