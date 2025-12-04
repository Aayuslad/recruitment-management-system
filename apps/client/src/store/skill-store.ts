import type { Skill } from '@/types/skill-types';
import type { StateCreator } from 'zustand';

export type SkillStoreSliceType = {
    skillEditTarget: Skill | null;
    skillEditDialog: boolean;
    skillDeleteTargetId: string | null;
    skillDeleteDialog: boolean;

    openSkillEditDialog: (skill: Skill | null) => void;
    closeSkillEditDialog: () => void;
    setSkillEditDialog: (state: boolean) => void;
    updateSkillEditTarget: (updates: Partial<Skill>) => void;

    openSkillDeleteDialog: (skillId: string) => void;
    closeSkillDeleteDialog: () => void;
    setSkillDeleteDialog: (state: boolean) => void;
};

export const createSkillSlice: StateCreator<SkillStoreSliceType> = (set) => ({
    skillEditTarget: null,
    skillEditDialog: false,
    skillDeleteTargetId: null,
    skillDeleteDialog: false,

    openSkillEditDialog: (skill) => {
        set({ skillEditTarget: skill, skillEditDialog: true });
    },

    closeSkillEditDialog: () =>
        set({ skillEditTarget: null, skillEditDialog: false }),

    setSkillEditDialog: (state) => set(() => ({ skillEditDialog: state })),

    updateSkillEditTarget: (updates: Partial<Skill>) =>
        set((state) => ({
            skillEditTarget: state.skillEditTarget
                ? { ...state.skillEditTarget, ...updates }
                : null,
        })),

    openSkillDeleteDialog: (skillId) => {
        set({ skillDeleteTargetId: skillId, skillDeleteDialog: true });
    },

    closeSkillDeleteDialog: () =>
        set({ skillDeleteTargetId: null, skillDeleteDialog: false }),

    setSkillDeleteDialog: (state) => set(() => ({ skillDeleteDialog: state })),
});
