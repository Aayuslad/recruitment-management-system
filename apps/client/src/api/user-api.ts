import type {
    CreateUserCommandCorrected,
    EditUserRolesCommandCorrected,
    LoginUserCommandCorrected,
    RegisterUserCommandCorrected,
    User,
} from '@/types/user-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';

export function useRegisterUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: RegisterUserCommandCorrected
        ): Promise<void> => {
            await axios.post('/user/register', payload);
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
            const { data } = await axios.post('/user/user-profile', payload);
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
            await axios.post('/user/login', payload);
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
            await axios.post('/user/logout');
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
            await axios.put(`/user/${payload.userId}/roles`, payload);
        },
        onSuccess: () => {
            toast.success('User Roles Updated');
            //TODO add invalidation here, when you areate get all users route for admin
            queryClient.invalidateQueries({ queryKey: [''] });
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

    return useQuery({
        queryKey: ['user'],
        queryFn: async (): Promise<User | undefined> => {
            try {
                const { data } = await axios.get('/user/me');
                return data;
            } catch {
                navigate('/login');
            }
        },
    });
}
