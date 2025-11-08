import { useGetUser } from '@/api/user';
import AppHeader from '@/components/app-header';
import { AppSidebar } from '@/components/app-sidebar';
import { SidebarInset, SidebarProvider } from '@/components/ui/sidebar';
import { Outlet } from 'react-router-dom';
import LoadingPage from './loading-page';

function HomeLayout() {
    const { isPending } = useGetUser();

    if (isPending) {
        return <LoadingPage />;
    }

    return (
        <SidebarProvider>
            <AppSidebar />
            <SidebarInset>
                <AppHeader />
                <Outlet />
            </SidebarInset>
        </SidebarProvider>
    );
}

export default HomeLayout;
