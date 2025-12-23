import { create } from 'zustand';
import { createUserSlice, type UserStoreSliceType } from './user-store';
import {
    createUiSlice,
    SIDEBAR_STATE_KEY,
    THEME_KEY,
    type UiStoreSliceType,
} from './ui-store';
import { createSkillSlice, type SkillStoreSliceType } from './skill-store';
import {
    createDocTypeSlice,
    type DocumentTypeStoreSliceType,
} from './doc-type-store';
import {
    createDesignationSlice,
    type DesignationStoreSliceType,
} from './designation-store';
import {
    createJobApplicationSlice,
    type JobApplicationStoreSliceType,
} from './job-application-store';

type AppStoreTupe = UserStoreSliceType &
    UiStoreSliceType &
    SkillStoreSliceType &
    DocumentTypeStoreSliceType &
    DesignationStoreSliceType &
    JobApplicationStoreSliceType;

export const useAppStore = create<AppStoreTupe>((...args) => ({
    ...createUserSlice(...args),
    ...createUiSlice(...args),
    ...createSkillSlice(...args),
    ...createDocTypeSlice(...args),
    ...createDesignationSlice(...args),
    ...createJobApplicationSlice(...args),
}));

// updating configs from localstorage
if (typeof window !== 'undefined') {
    const theme = localStorage.getItem(THEME_KEY);
    useAppStore.getState().setTheme(theme as 'light' | 'dark');

    const sidebarState = localStorage.getItem(SIDEBAR_STATE_KEY);
    if (sidebarState) {
        useAppStore
            .getState()
            .setSidebarState(sidebarState as 'opend' | 'closed');
    }
}
