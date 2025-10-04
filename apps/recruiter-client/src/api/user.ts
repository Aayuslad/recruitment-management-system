import type {
    CreateUserCommandCorrected,
    LoginUserCommandCorrected,
    RegisterUserCommandCorrected,
    UserDTO,
} from '@/types/user-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';

// register user
export function useRegisterUser() {
    const navigate = useNavigate();

    return useMutation({
        mutationFn: async (
            payload: RegisterUserCommandCorrected
        ): Promise<undefined> => {
            await axios.post('/User/register', payload);
        },
        onSuccess: () => {
            navigate('/create-user');
        },
        onError: (error: any) => {
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
        mutationFn: async (payload: CreateUserCommandCorrected) => {
            const { data } = await axios.post('/User/createUser', payload);
            return data;
        },
        onSuccess: () => {
            navigate('/dashboard');
        },
        onError: (error: any) => {
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
        ): Promise<undefined> => {
            await axios.post('/User/login', payload);
        },
        onSuccess: () => {
            navigate('/dashboard');
        },
        onError: (error: any) => {
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
            await axios.post('/User/logout');
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
                const { data } = await axios.get('/User/me');
                return data;
            } catch {
                navigate('/login');
            }
        },
    });
}
