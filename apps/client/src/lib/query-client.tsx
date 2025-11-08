import { QueryClient } from '@tanstack/react-query';

export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            // retry failed requests 1 time
            retry: 1,
            // stale time to avoid refetching immediately
            staleTime: 1000 * 60 * 5, // 5 minutes
            // error handling globally
            //   onError: (err) => console.error("Query error:", err),
            refetchOnWindowFocus: false, // avoid auto refetch when switching tabs
        },
    },
});
