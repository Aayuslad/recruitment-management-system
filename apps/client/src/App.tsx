import { Toaster } from '@/components/ui/sonner';
import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Route, Routes } from 'react-router-dom';
import { queryClient } from './lib/query-client';
import { EmployeesPage } from './pages/admin/employees-page';
import { RolesPage } from './pages/admin/roles-page';
import { UsersPage } from './pages/admin/users-page';
import CreateUserPage from './pages/auth/create-user-page';
import LoginPage from './pages/auth/login-page';
import RegistrationPage from './pages/auth/registration-page';
import { Index as CandidatesIndex } from './pages/candidate';
import { DesignationsPage } from './pages/configuration/designations-page';
import { DocumentTypesPage } from './pages/configuration/document-types-page';
import { SkillsPage } from './pages/configuration/skills-page';
import { Index as EventsIndex } from './pages/event';
import { DashboardPage } from './pages/dashboard-page';
import HomeLayout from './pages/home-layout';
import { Index as InterviewsIndex } from './pages/interview';
import { Index as JobApplicationsIndex } from './pages/jobApplication';
import { Index as JobOpeningsIndex } from './pages/jobOpening';
import LandingPage from './pages/landing-page';
import NotFoundPage from './pages/not-found-page';
import { Index as PositionsIndex } from './pages/position';
import { Index as ReportsAndAnalyticsIndex } from './pages/ReportsAndAnalytics';
import { PositionBatchDetailsPage } from './pages/position/position-batch-details-page';

function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <Routes>
                <Route path="/" element={<LandingPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegistrationPage />} />
                <Route path="/create-user" element={<CreateUserPage />} />
                <Route path="/" element={<HomeLayout />}>
                    <Route index path="dashboard" element={<DashboardPage />} />
                    <Route path="job-openings" element={<JobOpeningsIndex />} />
                    <Route path="candidates" element={<CandidatesIndex />} />
                    <Route
                        path="job-applications"
                        element={<JobApplicationsIndex />}
                    />
                    <Route path="interviews" element={<InterviewsIndex />} />

                    <Route path="positions">
                        <Route index element={<PositionsIndex />} />
                        <Route
                            path="batch/:id"
                            element={<PositionBatchDetailsPage />}
                        />
                    </Route>
                    <Route path="events" element={<EventsIndex />} />
                    <Route
                        path="reports-and-analytics"
                        element={<ReportsAndAnalyticsIndex />}
                    />

                    <Route path="configuration">
                        <Route path="skills" element={<SkillsPage />} />
                        <Route
                            path="designations"
                            element={<DesignationsPage />}
                        />
                        <Route
                            path="document-types"
                            element={<DocumentTypesPage />}
                        />
                    </Route>
                    <Route path="admin">
                        <Route path="users" element={<UsersPage />} />
                        <Route path="roles" element={<RolesPage />} />
                        <Route path="employees" element={<EmployeesPage />} />
                    </Route>
                </Route>
                <Route path="*" element={<NotFoundPage />} />
            </Routes>

            {process.env.NODE_ENV === 'development' && <ReactQueryDevtools />}

            <Toaster position="bottom-right" />
        </QueryClientProvider>
    );
}

export default App;
