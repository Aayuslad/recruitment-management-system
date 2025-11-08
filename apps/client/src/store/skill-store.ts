import type { SkillDTO } from '@/types/skill-types';
import type { StateCreator } from 'zustand';

export type SkillStoreSliceType = {
    editingSkill: SkillDTO | null;
    editDialog: boolean;

    openEditDialog: (skill: SkillDTO) => void;
    closeEditDialog: () => void;
    setEditDialog: (state: boolean) => void;
    updateEditingSkill: (updates: Partial<SkillDTO>) => void;
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

    updateEditingSkill: (updates: Partial<SkillDTO>) =>
        set((state) => ({
            editingSkill: state.editingSkill
                ? { ...state.editingSkill, ...updates }
                : null,
        })),
});
