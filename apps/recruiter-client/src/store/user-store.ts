import type { StateCreator } from 'zustand';

// eslint-disable-next-line
export type UserStoreSliceType = {};

export const createUserSlice: StateCreator<UserStoreSliceType> = (
    //@ts-expect-error it is not used
    // eslint-disable-next-line
    set,
    //@ts-expect-error it is not used
    // eslint-disable-next-line
    get
) => ({});
