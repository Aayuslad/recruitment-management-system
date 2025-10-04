import { Toaster } from '@/components/ui/sonner';
import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Route, Routes } from 'react-router-dom';
import { queryClient } from './lib/query-client';
import CandidatesPage from './pages/feature/candidates-page';
import CreateUserPage from './pages/auth/create-user-page';
import DashBoardPage from './pages/feature/dashboard-page';
import HomeLayout from './pages/home-layout';
import JobsPage from './pages/feature/jobs-page';
import LandingPage from './pages/landing-page';
import LoginPage from './pages/auth/login-page';
import NotFoundPage from './pages/not-found-page';
import RegistrationPage from './pages/auth/registration-page';
import CandidateSubPage1 from './pages/feature/candidates-sub-page-1';
import AdminSettingsPage from './pages/feature/admin-settings-page';
import DocumentVerificationPage from './pages/feature/document-verification-page';
import FeedbackAndEvaluationPage from './pages/feature/feedback-and-evaluation-page';
import InterviewSchedulingPage from './pages/feature/interview-scheduling-page';
import NotificationAndTasksPage from './pages/feature/notification-and-tasks-page';
import ReportsAndAnalyticsPage from './pages/feature/reports-and-analytics-page';
import ScreeningAndShortlistingPage from './pages/feature/screening-and-shortlisting-page';

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
                    <Route path="jobs" element={<JobsPage />} />
                    <Route path="candidates" element={<CandidatesPage />} />
                    <Route
                        path="candidates/candidate-sub-page-1"
                        element={<CandidateSubPage1 />}
                    />
                    <Route
                        path="admin-settings"
                        element={<AdminSettingsPage />}
                    />
                    <Route
                        path="document-verification"
                        element={<DocumentVerificationPage />}
                    />
                    <Route
                        path="feedback-and-evaluation"
                        element={<FeedbackAndEvaluationPage />}
                    />
                    <Route
                        path="interview-scheduling"
                        element={<InterviewSchedulingPage />}
                    />
                    <Route
                        path="notification-and-tasks"
                        element={<NotificationAndTasksPage />}
                    />
                    <Route
                        path="reports-and-analytics"
                        element={<ReportsAndAnalyticsPage />}
                    />
                    <Route
                        path="screening-and-shortlisting"
                        element={<ScreeningAndShortlistingPage />}
                    />
                </Route>
                <Route path="*" element={<NotFoundPage />} />
            </Routes>

            {process.env.NODE_ENV === 'development' && <ReactQueryDevtools />}

            <Toaster position="top-center" />
        </QueryClientProvider>
    );
}

export default App;
