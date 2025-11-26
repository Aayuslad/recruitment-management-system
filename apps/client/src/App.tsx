import { Toaster } from '@/components/ui/sonner';
import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Route, Routes } from 'react-router-dom';
import { queryClient } from './lib/query-client';
import CreateUserPage from './pages/auth/create-user-page';
import LoginPage from './pages/auth/login-page';
import RegistrationPage from './pages/auth/registration-page';
import DashBoardPage from './pages/feature/dashboard-page';
import HomeLayout from './pages/home-layout';
import LandingPage from './pages/landing-page';
import NotFoundPage from './pages/not-found-page';

function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <Routes>
                <Route path="/" element={<LandingPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegistrationPage />} />
                <Route path="/create-user" element={<CreateUserPage />} />
                <Route path="/" element={<HomeLayout />}>
                    <Route path="dashboard" element={<DashBoardPage />} />
                </Route>
                <Route path="*" element={<NotFoundPage />} />
            </Routes>

            {process.env.NODE_ENV === 'development' && <ReactQueryDevtools />}

            <Toaster position="top-center" />
        </QueryClientProvider>
    );
}

export default App;
