import type { Employee } from '@/types/employee-types';
import { useQuery } from '@tanstack/react-query';
import axios from 'axios';

export function useGetEmployees() {
    return useQuery({
        queryKey: ['employees'],
        queryFn: async (): Promise<Employee[]> => {
            const { data } = await axios.get('/employee');
            return data;
        },
    });
}