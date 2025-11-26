import type { Skill } from '@/types/skill-types';
import type { StateCreator } from 'zustand';

export type SkillStoreSliceType = {
    editingSkill: Skill | null;
    editDialog: boolean;

    openEditDialog: (skill: Skill) => void;
    closeEditDialog: () => void;
    setEditDialog: (state: boolean) => void;
    updateEditingSkill: (updates: Partial<Skill>) => void;
};

export const createSkillSlice: StateCreator<SkillStoreSliceType> = (set) => ({
    editingSkill: null,
    editDialog: false,

    openEditDialog: (skill) => {
        set({ editingSkill: skill });
        set({ editDialog: true });
    },

    closeEditDialog: () => set({ editingSkill: null, editDialog: false }),

    setEditDialog: (state) =>
        set(() => ({
            editDialog: state,
        })),

    updateEditingSkill: (updates: Partial<Skill>) =>
        set((state) => ({
            editingSkill: state.editingSkill
                ? { ...state.editingSkill, ...updates }
                : null,
        })),
});
