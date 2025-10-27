import type {
    CreateUserCommandCorrected,
    LoginUserCommandCorrected,
    RegisterUserCommandCorrected,
    UserDTO,
} from '@/types/user-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';

// register user
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

// create user
export function useCreateUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: CreateUserCommandCorrected
        ): Promise<void> => {
            const { data } = await axios.post('/user/createUser', payload);
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

// login user
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

// logout user
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

// get user
export function useGetUser() {
    const navigate = useNavigate();

    return useQuery({
        queryKey: ['user'],
        queryFn: async (): Promise<UserDTO | undefined> => {
            try {
                const { data } = await axios.get('/user/me');
                return data;
            } catch {
                navigate('/login');
            }
        },
    });
}
