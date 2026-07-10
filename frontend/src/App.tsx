import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import ConnectionDetailPage from "./pages/ConnectionDetailPage";
import ConnectionsPage from "./pages/ConnectionsPage";
import CreateConnectionPage from "./pages/CreateConnectionPage";

const App = () => {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<ConnectionsPage />} />
				<Route path="/connections/new" element={<CreateConnectionPage />} />
				<Route
					path="/connections/:locationId"
					element={<ConnectionDetailPage />}
				/>
				<Route path="*" element={<Navigate to="/" replace />} />
			</Routes>
		</BrowserRouter>
	);
};

export default App;
