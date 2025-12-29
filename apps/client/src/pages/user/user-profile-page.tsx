import { useGetUser } from '@/api/user-api';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Spinner } from '@/components/ui/spinner';

export function UserProfilePage() {
    const { data: user, isLoading, isError } = useGetUser();

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Profile</div>;

    return (
        <div className="flex justify-center">
            <div className="w-[700px] py-10 space-y-20">
                <div className="flex items-center gap-6 px-1 py-1.5 text-left text-sm">
                    <Avatar className="h-14 w-14 rounded-lg">
                        <AvatarFallback className="rounded-lg text-3xl capitalize">
                            {user?.userName[0]}
                        </AvatarFallback>
                    </Avatar>
                    <div className="grid flex-1 text-left ">
                        <div className="text-3xl">
                            {user?.firstName} {user?.middleName}{' '}
                            {user?.lastName}
                        </div>
                        <div>{user?.userName}</div>
                    </div>
                </div>
                <div className="space-y-6 px-1 border-t pt-6">
                    <div className="grid grid-cols-3 gap-8">
                        <div>
                            <p className="text-sm font-semibold text-white mb-2">
                                Roles
                            </p>
                            <p className="text-base text-white">
                                {user?.roles.map((r) => r.name).join(', ')}
                            </p>
                        </div>
                        <div>
                            <p className="text-sm font-semibold text-white mb-2">
                                Email
                            </p>
                            <p className="text-base text-white">
                                {user?.email}
                            </p>
                        </div>
                        <div>
                            <p className="text-sm font-semibold text-white mb-2">
                                Contact Number
                            </p>
                            <p className="text-base text-white">
                                {user?.contactNumber}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
