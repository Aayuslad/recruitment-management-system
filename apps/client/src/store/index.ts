import { create } from 'zustand';
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
import { createAdminSlice, type AdminStoreSliceType } from './admin-store';
import { createAuthSlice, type AuthStoreSliceType } from './auth-store';

type AppStoreType = UiStoreSliceType &
    SkillStoreSliceType &
    DocumentTypeStoreSliceType &
    DesignationStoreSliceType &
    JobApplicationStoreSliceType &
    AdminStoreSliceType &
    AuthStoreSliceType;

export const useAppStore = create<AppStoreType>((...args) => ({
    ...createUiSlice(...args),
    ...createSkillSlice(...args),
    ...createDocTypeSlice(...args),
    ...createDesignationSlice(...args),
    ...createJobApplicationSlice(...args),
    ...createAdminSlice(...args),
    ...createAuthSlice(...args),
}));

// updating configs from localstorage
if (typeof window !== 'undefined') {
    const theme = localStorage.getItem(THEME_KEY);
    useAppStore.getState().setTheme(theme as 'light' | 'dark');

    const sidebarState = localStorage.getItem(SIDEBAR_STATE_KEY);
    if (sidebarState) {
        useAppStore
            .getState()
            .setSidebarState(sidebarState as 'opened' | 'closed');
    }
}
