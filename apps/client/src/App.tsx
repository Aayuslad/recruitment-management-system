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
import { JobOpeningDetailPage } from './pages/jobOpening/job-opening-details-page';
import { CandidateDetailsPage } from './pages/candidate/candidate-details-page';
import { JobApplicationDetailsPage } from './pages/jobApplication/job-application-details-page';
import { InterviewDetailsPage } from './pages/interview/interview-details-page';
import { Index as ScreeningsIndex } from './pages/screenings';
import { ScreeningApplicationDetailsPage } from './pages/screenings/screening-application-details-page';
import { Index as VerificationsIndex } from './pages/verifications';
import { VerificationCandidateDetailsPage } from './pages/verifications/verification-candidate-details-page';
import { UserProfilePage } from './pages/user/user-profile-page';

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
                    <Route path="job-openings">
                        <Route index element={<JobOpeningsIndex />} />
                        <Route
                            path="opening/:id"
                            element={<JobOpeningDetailPage />}
                        />
                    </Route>
                    <Route path="candidates">
                        <Route index element={<CandidatesIndex />} />
                        <Route
                            path="candidate/:id"
                            element={<CandidateDetailsPage />}
                        />
                    </Route>
                    <Route path="job-applications">
                        <Route index element={<JobApplicationsIndex />} />
                        <Route
                            path="application/:id"
                            element={<JobApplicationDetailsPage />}
                        />
                    </Route>
                    <Route path="interviews">
                        <Route index element={<InterviewsIndex />} />
                        <Route
                            path="interview/:id"
                            element={<InterviewDetailsPage />}
                        />
                    </Route>
                    <Route path="screenings">
                        <Route index element={<ScreeningsIndex />} />
                        <Route
                            path="application/:id"
                            element={<ScreeningApplicationDetailsPage />}
                        />
                    </Route>
                    <Route path="verifications">
                        <Route index element={<VerificationsIndex />} />
                        <Route
                            path="candidate/:id"
                            element={<VerificationCandidateDetailsPage />}
                        />
                    </Route>
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
                    <Route
                        path="user-profile/:id?"
                        element={<UserProfilePage />}
                    />
                </Route>
                <Route path="*" element={<NotFoundPage />} />
            </Routes>

            {process.env.NODE_ENV === 'development' && <ReactQueryDevtools />}

            <Toaster position="bottom-right" />
        </QueryClientProvider>
    );
}

export default App;
