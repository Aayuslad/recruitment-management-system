import type { Skill } from '@/types/skill-types';
import type { StateCreator } from 'zustand';

export type SkillStoreSliceType = {
    editingSkill: Skill | null;
    editDialog: boolean;
    deletingSkillId: string | null;
    deleteDialog: boolean;

    openEditDialog: (skill: Skill) => void;
    closeEditDialog: () => void;
    setEditDialog: (state: boolean) => void;
    updateEditingSkill: (updates: Partial<Skill>) => void;
    openDeleteDialog: (skillId: string) => void;
    closeDeleteDialog: () => void;
    setDeleteDialog: (state: boolean) => void;
};

export const createSkillSlice: StateCreator<SkillStoreSliceType> = (set) => ({
    editingSkill: null,
    editDialog: false,
    deletingSkillId: null,
    deleteDialog: false,

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

    openDeleteDialog: (skillId) => {
        set({ deletingSkillId: skillId });
        set({ deleteDialog: true });
    },

    closeDeleteDialog: () =>
        set({ deletingSkillId: null, deleteDialog: false }),

    setDeleteDialog: (state) =>
        set(() => ({
            deleteDialog: state,
        })),
});
