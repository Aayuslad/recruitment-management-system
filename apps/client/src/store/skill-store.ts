import type { Skill } from '@/types/skill-types';
import type { StateCreator } from 'zustand';

export type SkillStoreSliceType = {
    skillEditTarget: Skill | null;
    isSkillEditDialogOpen: boolean;
    skillDeleteTargetId: string | null;
    isSkillDeleteDialogOpen: boolean;

    openSkillEditDialog: (skill: Skill | null) => void;
    closeSkillEditDialog: () => void;
    setSkillEditDialogOpen: (state: boolean) => void;
    updateSkillEditTarget: (updates: Partial<Skill>) => void;

    openSkillDeleteDialog: (skillId: string) => void;
    closeSkillDeleteDialog: () => void;
    setSkillDeleteDialogOpen: (state: boolean) => void;
};

export const createSkillSlice: StateCreator<SkillStoreSliceType> = (set) => ({
    skillEditTarget: null,
    isSkillEditDialogOpen: false,
    skillDeleteTargetId: null,
    isSkillDeleteDialogOpen: false,

    openSkillEditDialog: (skill) => {
        set({ skillEditTarget: skill, isSkillEditDialogOpen: true });
    },

    closeSkillEditDialog: () =>
        set({ skillEditTarget: null, isSkillEditDialogOpen: false }),

    setSkillEditDialogOpen: (state) =>
        set(() => ({ isSkillEditDialogOpen: state })),

    updateSkillEditTarget: (updates: Partial<Skill>) =>
        set((state) => ({
            skillEditTarget: state.skillEditTarget
                ? { ...state.skillEditTarget, ...updates }
                : null,
        })),

    openSkillDeleteDialog: (skillId) => {
        set({ skillDeleteTargetId: skillId, isSkillDeleteDialogOpen: true });
    },

    closeSkillDeleteDialog: () =>
        set({ skillDeleteTargetId: null, isSkillDeleteDialogOpen: false }),

    setSkillDeleteDialogOpen: (state) =>
        set(() => ({ isSkillDeleteDialogOpen: state })),
});
