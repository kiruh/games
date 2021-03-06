import React from "react";
import axios from "axios";
import PropTypes from "prop-types";

import { getHeaders } from "~/utils";

class GenreForm extends React.Component {
	static renderSaveButton() {
		return (
			<button type="submit" className="btn btn-primary">
				Save
			</button>
		);
	}

	constructor(props) {
		super(props);
		this.state = this.getInitialState(props);
	}

	getInitialState() {
		const { genre } = this.props;
		return {
			name: genre ? genre.name : "",
			description: genre ? genre.description : "",
			error: null,
		};
	}

	componentDidMount() {
		this.mounted = true;
	}

	componentWillUnmount() {
		this.mounted = false;
	}

	onChangeInput(value, field) {
		this.setState({
			[field]: value,
			error: null,
		});
	}

	onSave() {
		if (!this.state.name || !this.state.description) {
			const error = "Please fill in all fields";
			this.setState({ error });
			return;
		}
		this.saveGenre();
	}

	async saveGenre() {
		const { genre } = this.props;
		try {
			const { name, description } = this.state;
			if (genre) {
				const { id } = genre;
				await axios.put(
					`/api/genres/${id}`,
					{ name, description },
					{ headers: getHeaders() },
				);
			} else {
				await axios.post(
					"/api/genres/",
					{ name, description },
					{ headers: getHeaders() },
				);
			}
			if (this.mounted) {
				await this.props.onSave();

				if (this.mounted) {
					this.setState({
						name: "",
						description: "",
						error: null,
					});
				}
			}
		} catch (error) {
			const errorMsg = JSON.stringify(error.response.data);
			if (this.mounted) {
				this.setState({ error: errorMsg });
			}
		}
	}

	renderValueInput() {
		return (
			<div className="form-group my-0">
				<input
					className="form-control bg-secondary text-white"
					placeholder="Title"
					value={this.state.name}
					onChange={event => {
						this.onChangeInput(event.target.value, "name");
					}}
					maxLength="100"
				/>
			</div>
		);
	}

	renderDescriptionInput() {
		return (
			<div className="form-group">
				<textarea
					className="form-control bg-light text-white"
					placeholder="Description"
					value={this.state.description}
					onChange={event => {
						this.onChangeInput(event.target.value, "description");
					}}
					rows="5"
				/>
			</div>
		);
	}

	renderError() {
		const { error } = this.state;
		if (!error) return null;
		return <p className="card-text text-danger">{error}</p>;
	}

	renderCancelButton() {
		return (
			<button
				type="button"
				className="btn btn-secondary mr-2"
				onClick={() => {
					const { onCancel } = this.props;
					if (onCancel) onCancel();
					else this.setState(this.getInitialState());
				}}
			>
				Cancel
			</button>
		);
	}

	render() {
		return (
			<div className="col-md-3">
				<div className="card text-white bg-light my-2">
					<form
						onSubmit={event => {
							event.preventDefault();
							this.onSave();
						}}
					>
						<div className="card-header">
							{this.renderValueInput()}
						</div>
						<div className="card-body">
							{this.renderDescriptionInput()}
							{this.renderError()}
							{this.renderCancelButton()}
							{GenreForm.renderSaveButton()}
						</div>
					</form>
				</div>
			</div>
		);
	}
}

GenreForm.propTypes = {
	genre: PropTypes.objectOf(PropTypes.any),
	onSave: PropTypes.func.isRequired,
	onCancel: PropTypes.func,
};

export default GenreForm;
