import { useAppStore } from '@/store';
import type {
    CreateUserCommandCorrected,
    EditUserRolesCommandCorrected,
    LoginUserCommandCorrected,
    RegisterUserCommandCorrected,
    User,
    UsersDetail,
    UsersSummary,
} from '@/types/user-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';
import { useShallow } from 'zustand/react/shallow';

export function useRegisterUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: RegisterUserCommandCorrected
        ): Promise<void> => {
            await axios.post('/users/register', payload);
        },
        onSuccess: () => {
            navigate('/create-user');
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Register failed.'
            );
            console.error('Register failed:', error);
        },
    });
}

export function useCreateUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: CreateUserCommandCorrected
        ): Promise<void> => {
            const { data } = await axios.post('/users/user-profile', payload);
            return data;
        },
        onSuccess: () => {
            navigate('/dashboard');
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'User Creation failed.'
            );
            console.error('User Creation failed:', error);
        },
    });
}

export function useLoginUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: LoginUserCommandCorrected
        ): Promise<void> => {
            await axios.post('/users/login', payload);
        },
        onSuccess: () => {
            navigate('/dashboard');
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Login failed. Please check your credentials.'
            );
            console.error('Login failed:', error);
        },
    });
}

export function useLogoutUser() {
    const queryClient = useQueryClient();
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async () => {
            await axios.post('/users/logout');
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['user'] });
            navigate('/login');
        },
    });
}

export function useEditUserRoles() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditUserRolesCommandCorrected
        ): Promise<void> => {
            await axios.put(`/users/${payload.userId}/roles`, payload);
        },
        onSuccess: () => {
            toast.success('User Roles Updated');
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'User Roles Not Updated'
            );
            console.error('User role update failed:', error);
        },
    });
}

export function useGetUser() {
    const navigate = useNavigate();
    const { setUserRoles } = useAppStore(
        useShallow((s) => ({
            setUserRoles: s.setUserRoles,
        }))
    );

    return useQuery({
        queryKey: ['user'],
        queryFn: async (): Promise<User | undefined> => {
            try {
                const { data } = await axios.get('/users/me');
                setUserRoles(data.roles.map((r: { name: string }) => r.name));
                return data;
            } catch {
                navigate('/login');
            }
        },
    });
}

export function useGetUsers() {
    return useQuery({
        queryKey: ['users'],
        queryFn: async (): Promise<UsersDetail[]> => {
            const { data } = await axios.get('/users');
            return data;
        },
    });
}

export function useGetUsersSummary() {
    return useQuery({
        queryKey: ['users-summary'],
        queryFn: async (): Promise<UsersSummary[]> => {
            const { data } = await axios.get('/users/summary');
            return data;
        },
    });
}
